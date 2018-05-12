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
    @IBOutlet weak var portraitView: PortraitView!
    
    var motionManager = CMMotionManager()
    var volumeButtonHandler: JPSVolumeButtonHandler?
    var rotationSocket = WebSocket(url: URL(string: "ws://10.0.1.134:9001/M_Rotation")!)
    var inputSocket = WebSocket(url: URL(string: "ws://10.0.1.134:9001/M_Input")!)
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        setNeedsUpdateOfScreenEdgesDeferringSystemGestures()
        
        scribbleView.delegate = self
        portraitView.delegate = self
        
        let upBlock = { () -> Void in
            self.sendText(text: "UP_PRESS")
            
            if UIDevice.current.orientation == UIDeviceOrientation.portrait {
                self.resetConnection()
            }
        }
        
        let downBlock = { () -> Void in
            self.sendText(text: "DOWN_PRESS")
        }
        
        volumeButtonHandler = JPSVolumeButtonHandler(up: upBlock, downBlock: downBlock)
        volumeButtonHandler?.start(true)
        
        resetConnection()
    }
    
    func resetConnection() {
        rotationSocket.disconnect()
        inputSocket.disconnect()
        motionManager.stopDeviceMotionUpdates()

        // The hackiest of them all
        usleep(250000)
        
        rotationSocket.connect()
        inputSocket.connect()

        motionManager.startDeviceMotionUpdates(to: OperationQueue.current!, withHandler: {
            (deviceMotion: CMDeviceMotion?, error: Error?) -> Void in
            self.rotationSocket.write(string: "\(deviceMotion!.attitude.pitch);\(deviceMotion!.attitude.yaw);\(deviceMotion!.attitude.roll)")
        })
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    override func viewWillTransition(to size: CGSize, with coordinator: UIViewControllerTransitionCoordinator) {
        if UIDevice.current.orientation.isLandscape {
            self.portraitView.isHidden = true
            self.sendText(text: "ORIENTATION_LANDSCAPE")
        } else {
            self.portraitView.isHidden = false
            self.sendText(text: "ORIENTATION_PORTRAIT")
        }
    }
    
    func sendText(text: String) {
        print(text)
        self.inputSocket.write(string: text)
    }
    
    override func preferredScreenEdgesDeferringSystemGestures() -> UIRectEdge {
        return [.bottom,.top]
    }
}
