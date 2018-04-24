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
    
    var onTraceRecognized : ((String) -> ())?
    let googleAPIEndpoint : String = "https://www.google.com.tw/inputtools/request?ime=handwriting&app=mobilesearch&cs=1&oe=UTF-8"
    
    func getTraceValue (trace : [[[Float]]]) {
        
        let parameters : Parameters = [
            "options": "enable_pre_space",
            "requests": [[
                "writing_guide": [
                    "writing_area_width": 800,
                    "writing_area_height": 800
                ],
                "ink": trace,
                "language": "en"
                ]]
        ]
        
        Alamofire.request(googleAPIEndpoint, method: .post, parameters: parameters, encoding: JSONEncoding.default).responseJSON { response in

            // Get the body of JSON response
            if let data = response.data {
                let jsonResponse = JSON.init(parseJSON: String(data: data, encoding: .utf8)!)
                
                self.onTraceRecognized?(jsonResponse[1][0][1][0].string!)
            }
        }
    }
}

