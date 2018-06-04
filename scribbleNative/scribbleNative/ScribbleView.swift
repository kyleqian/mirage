//
//  ScribbleView.swift
//  scribbleNative
//
//  Created by Christian Valadez on 4/8/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation
import UIKit
import SwiftyJSON

class ScribbleView: UIView {
    weak var delegate: NetworkDelegate?
    
    var isDrawing = false
    var isDoubleTouching = false
    var isMultiTouching = false
    var lastPoint: CGPoint!
    var strokeColor: CGColor = UIColor.black.cgColor
    var strokes = [Stroke]()
    var recognitionAPI = RecognitionAPI()
    
    // This is for now to send to the recognitionAPI. Should be integrated with strokes
    var trace : [[[Float]]] = []
//    var traceForStormi : [[Float]] = []
//    var curPath = Path()
    
    // For timer:
//    DispatchQueue.main.asyncAfter(deadline: .now() + 2) { // change 2 to desired number of seconds
//    // Your code with delay
//    }
    
    //Or:
    //Number of seconds.
    var timer: Timer? = nil;
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        // TODO
        let numTotalTouches = (event?.allTouches?.count)!
        if numTotalTouches > 1 {
            erase()
            resetTrace()
            isDrawing = false
            
            // Can upgrade but not downgrade
            if numTotalTouches == 2 && !isMultiTouching {
                isDoubleTouching = true
            } else if numTotalTouches > 2 {
                isMultiTouching = true
                isDoubleTouching = false
            }
            
            return
        }
        
        guard !isDrawing else { return }
        isDrawing = true

        // Hmm, why are both this and the prev check necessary?
        guard let touch = touches.first else { return }
        
        let currentPoint = touch.location(in: self)
        lastPoint = currentPoint

        updateTrace(point: currentPoint)
        self.delegate?.sendStrokes(strokes: self.trace)
//        curPath = Path()
//        addToCurPath(point: currentPoint)
        
        timer?.invalidate()
    }
    
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        // TODO
        guard (event?.allTouches?.count)! <= 1 else { return }
        
        guard isDrawing else { return }
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        
//        addToCurPath(point: currentPoint)
        updateTrace(point: currentPoint)
        self.delegate?.sendStrokes(strokes: self.trace)

        
        lastPoint = currentPoint
        setNeedsDisplay()
    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        // TODO
        if !isDrawing {
            let numTouchesRemaining = (event?.allTouches?.count)! - touches.count
            if numTouchesRemaining == 0 {
                if isDoubleTouching {
                    self.delegate?.sendJSON(json: JSON(JSON_CONSTANTS.SEND_SPACE))
                } else if isMultiTouching {
                    self.delegate?.sendJSON(json: JSON(JSON_CONSTANTS.SEND_MULTI_SWIPE))
                }
                
                isDoubleTouching = false
                isMultiTouching = false
            }
        }
        
        guard isDrawing else { return }
        isDrawing = false
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        lastPoint = nil
        setNeedsDisplay()
        
        updateTrace(point: currentPoint)
//        addToCurPath(point: currentPoint)
//        addCurPathToTrace()
        
        // Send the strokes to Stormi
        delegate?.sendStrokes(strokes: trace)
        
        timer = Timer.scheduledTimer(withTimeInterval: 0.25, repeats: false) { (timer) in
            //Do stuff 15ms later
            self.erase()
            
            self.recognitionAPI.getTraceValue(trace: self.trace)
            self.resetTrace()
            self.delegate?.sendStrokes(strokes: self.trace)
            
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
    
//    func addToCurPath(point : CGPoint) {
//        curPath.xCoods.append(Float(point.x))
//        curPath.yCoods.append(Float(point.y))
//    }
    
//    func addCurPathToTraceForStormi() {
//        var path : [[Float]] = []
//        for ind in 0..<curPath.xCoods.count {
//            path.append([curPath.xCoods[ind], curPath.yCoods[ind]])
//        }
//        traceForStormi += path
//    }
    
    func updateTrace(point: CGPoint) {
        
        if trace.count == 0 { trace.append([[], []]) }
        
        // Get the most recent path and append the point there
        trace[trace.count - 1][0].append(Float(point.x))
        trace[trace.count - 1][1].append(Float(point.y))
    }
    
//    func addCurPathToTrace() {
//        trace.append([curPath.xCoods, curPath.yCoods])
////        addCurPathToTraceForStormi()
//
//        // reset current path
//        curPath = Path()
//    }
    
    func resetTrace() {
        trace = []
    }
    
    func erase() {
        strokes = []
        strokeColor = UIColor.black.cgColor
        setNeedsDisplay()
    }
}
