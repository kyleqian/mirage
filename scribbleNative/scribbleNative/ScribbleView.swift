//
//  ScribbleView.swift
//  scribbleNative
//
//  Created by Christian Valadez on 4/8/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation
import UIKit

class ScribbleView: UIView {
    weak var delegate: NetworkDelegate?
    
    var isDrawing = false
    var isMultiTouching = false
    var lastPoint: CGPoint!
    var strokeColor: CGColor = UIColor.black.cgColor
    var strokes = [Stroke]()
    var recognitionAPI = RecognitionAPI()
    
    // This is for now to send to the recognitionAPI. Should be integrated with strokes
    var trace : [[[Float]]] = []
    var curPath = Path()
    
    // For timer:
//    DispatchQueue.main.asyncAfter(deadline: .now() + 2) { // change 2 to desired number of seconds
//    // Your code with delay
//    }
    
    //Or:
    //Number of seconds.
    var timer: Timer? = nil;
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard !isDrawing else { return }
        isDrawing = true

        // Hmm, why are both this and the prev check necessary?
        guard let touch = touches.first else { return }
        
        // Hacky multi-touch handler
        if touches.count > 1 {
            guard !isMultiTouching else { return }
            isDrawing = false
            isMultiTouching = true
            return
        }
        
        let currentPoint = touch.location(in: self)
        lastPoint = currentPoint
        
        curPath = Path()
        addToCurPath(point: currentPoint)
        
        timer?.invalidate()
    }
    
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard isDrawing else { return }
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        
        addToCurPath(point: currentPoint)
        
        lastPoint = currentPoint
        setNeedsDisplay()
    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        // Hacky
        if isMultiTouching {
            self.delegate?.sendText(text: "SEND_SPACE")
            isMultiTouching = false
            return
        }
        
        guard isDrawing else { return }
        isDrawing = false
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        lastPoint = nil
        setNeedsDisplay()
        
        addToCurPath(point: currentPoint)
        addCurPathToTrace()
        
        timer = Timer.scheduledTimer(withTimeInterval: 0.25, repeats: false) { (timer) in
            //Do stuff 15ms later
            self.erase()
            
            self.recognitionAPI.getTraceValue(trace: self.trace)
            self.resetTrace()
            
            self.recognitionAPI.onTraceRecognized = { text in
                self.delegate?.sendText(text: text)
            }
        }
    }
    
    override func draw(_ rect: CGRect) {
        let context = UIGraphicsGetCurrentContext()
        context?.setLineWidth(4)
        context?.setLineCap(.round)
        for stroke in strokes {
            context?.beginPath()
            context?.move(to: stroke.startPoint)
            context?.addLine(to: stroke.endPoint)
            context?.setStrokeColor(stroke.color)
            context?.strokePath()
        }
    }
    
    ///// Helper Functions //////
    
    func addToCurPath(point : CGPoint) {
        curPath.xCoods.append(Float(point.x))
        curPath.yCoods.append(Float(point.y))
    }
    
    func addCurPathToTrace() {
        trace.append([curPath.xCoods, curPath.yCoods])
        // reset current path
        curPath = Path()
    }
    
    func resetTrace() {
        trace = []
    }
    
    func erase() {
        strokes = []
        strokeColor = UIColor.black.cgColor
        setNeedsDisplay()
    }
}
