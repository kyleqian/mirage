//
//  ViewController.swift
//  scribbleNative
//
//  Created by Christian Valadez on 4/8/18.
//  Copyright © 2018 Christian Valadez. All rights reserved.
//

import UIKit
import CoreMotion
import Starscream
import JPSVolumeButtonHandler

class ViewController: UIViewController, NetworkDelegate {
    @IBOutlet var scribbleView: ScribbleView!
    
    var motionManager = CMMotionManager()
    var volumeButtonHandler: JPSVolumeButtonHandler?
    var rotationSocket = WebSocket(url: URL(string: "ws://10.0.1.146:9001/M_Rotation")!)
    var inputSocket = WebSocket(url: URL(string: "ws://10.0.1.146:9001/M_Input")!)
    
    override func viewDidLoad() {
        super.viewDidLoad()
        scribbleView.delegate = self
        
        rotationSocket.connect()
        inputSocket.connect()
        
        motionManager.startDeviceMotionUpdates(to: OperationQueue.current!, withHandler: {
            (deviceMotion: CMDeviceMotion?, error: Error?) -> Void in
            self.rotationSocket.write(string: "\(deviceMotion!.attitude.pitch);\(deviceMotion!.attitude.yaw);\(deviceMotion!.attitude.roll)")
        })
        
        let upBlock = { () -> Void in
            self.inputSocket.write(string: "UP_PRESS")
        }
        
        let downBlock = { () -> Void in
            self.inputSocket.write(string: "DOWN_PRESS")
        }
        
        volumeButtonHandler = JPSVolumeButtonHandler(up: upBlock, downBlock: downBlock)
        volumeButtonHandler?.start(true)
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    func sendText(text: String) {
        print(text)
        self.inputSocket.write(string: text)
    }
}
