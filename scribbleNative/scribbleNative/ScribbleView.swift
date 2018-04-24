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
    var isDrawing = false
    var lastPoint: CGPoint!
    var strokeColor: CGColor = UIColor.black.cgColor
    var strokes = [Stroke]()
    var recognitionAPI = RecognitionAPI()
    
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
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        lastPoint = currentPoint
        print(currentPoint)
        
        timer?.invalidate()
    }
    
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard isDrawing else { return }
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        lastPoint = currentPoint
        setNeedsDisplay()
    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard isDrawing else { return }
        isDrawing = false
        guard let touch = touches.first else { return }
        let currentPoint = touch.location(in: self)
        let stroke = Stroke(startPoint: lastPoint, endPoint: currentPoint, color: strokeColor)
        strokes.append(stroke)
        lastPoint = nil
        print(currentPoint)
        setNeedsDisplay()
        
        timer = Timer.scheduledTimer(withTimeInterval: 0.25, repeats: true) { (timer) in
            //Do stuff 15ms later
            self.erase()
            
            let trace = [   //the trace is an array of strokes
                [   //a stroke is an array of pairs of captured (x, y) coordinates
                    [300, 310, 320, 330, 340], //x coordinate
                    [320, 320, 320, 320, 320]  //y coordinate
                    //each pair of (x, y) coordinates represents one sampling point
                ]
            ]
            
            self.recognitionAPI.getTraceValue(trace: trace)
            self.recognitionAPI.onTraceRecognized = { text in
                print("I got the text \(text)")
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
    
    func erase() {
        strokes = []
        strokeColor = UIColor.black.cgColor
        setNeedsDisplay()
    }
}
