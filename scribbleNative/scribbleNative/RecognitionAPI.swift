//
//  RecognitionAPI.swift
//  scribbleNative
//
//  Created by Tariq Patanam on 4/20/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation
import Alamofire
import SwiftyJSON

class RecognitionAPI {
    
    static let sharedInstance = RecognitionAPI()
    
    func sendTrace (trace : NSArray) {
        let trace = [   //the trace is an array of strokes
            [   //a stroke is an array of pairs of captured (x, y) coordinates
                [300, 310, 320, 330, 340], //x coordinate
                [320, 320, 320, 320, 320]  //y coordinate
                //each pair of (x, y) coordinates represents one sampling point
        ]
        ]
        
        let parameters : Parameters = [
            "options": "enable_pre_space",
            "requests": [[
                "writing_guide": [
                    "writing_area_width": 800,
                    "writing_area_height": 800
                ],
                "ink": trace,
                "language": "zh_TW"
                ]]
        ]
        
        Alamofire.request("https://www.google.com.tw/inputtools/request?ime=handwriting&app=mobilesearch&cs=1&oe=UTF-8", method: .post, parameters: parameters, encoding: JSONEncoding.default).responseJSON { response in
            let data = response.request?.httpBody
//            print("request body : \(String(data: data!, encoding: String.Encoding.utf8) as String!)")
//            print("Request: \(String(describing: response.request))")   // original url request
//            print("Response: \(String(describing: response.response))") // http url response
//            print("Result: \(response.result.value)")                         // response serialization result
            
            if let json = response.result.value {
                print("JSON: \(json)") // serialized json response
            }
        }
    }
}

