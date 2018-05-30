//
//  NetworkDelegate.swift
//  scribbleNative
//
//  Created by kyle on 4/23/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation
import SwiftyJSON

protocol NetworkDelegate: class {
    func sendText(text: String)
    func sendStrokes(strokes : [[[Float]]])
    func sendJSON(json : JSON)
}
