//
//  NetworkDelegate.swift
//  scribbleNative
//
//  Created by kyle on 4/23/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation

protocol NetworkDelegate: class {
    func sendText(text: String)
    func sendTrace(trace : [[[Float]]])
}
