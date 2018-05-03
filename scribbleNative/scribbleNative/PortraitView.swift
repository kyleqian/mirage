//
//  PortraitView.swift
//  scribbleNative
//
//  Created by kyle on 5/2/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import Foundation
import UIKit

class PortraitView: UIView {
    weak var delegate: NetworkDelegate?
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.delegate?.sendText(text: "PORTRAIT_TOUCH_BEGAN")
    }
    
//    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
//    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.delegate?.sendText(text: "PORTRAIT_TOUCH_ENDED")
    }
}
