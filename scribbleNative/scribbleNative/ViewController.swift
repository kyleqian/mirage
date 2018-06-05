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
import SwiftyJSON
import CocoaAsyncSocket

class ViewController: UIViewController, NetworkDelegate, GCDAsyncUdpSocketDelegate {
    @IBOutlet var scribbleView: ScribbleView!
    @IBOutlet weak var portraitView: PortraitView!
    
    var motionManager = CMMotionManager()
    var volumeButtonHandler: JPSVolumeButtonHandler?
    var rotationSocket: WebSocket!
    var inputSocket: WebSocket!
    var connected = false
    
    // UDP broadcast receiver
    var broadcastSocket: GCDAsyncUdpSocket!
    let broadcastPort: UInt16 = 9003
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        setNeedsUpdateOfScreenEdgesDeferringSystemGestures()
        
        scribbleView.delegate = self
        portraitView.delegate = self
        
//        // Receive IP address
//        receiveBroadcast()
        
//         TODO: Remove. Temp fix for Stanford Residences network
        connected = true
        initWebsocketConnections(ip: "10.31.242.110")
    }
    
    func receiveBroadcast() {
        broadcastSocket = GCDAsyncUdpSocket(delegate: self, delegateQueue: DispatchQueue.main)
        
        do {
            try broadcastSocket.bind(toPort: broadcastPort)
            try broadcastSocket.beginReceiving()
        } catch {
            print("ReceiveBroadcast error: \(error.localizedDescription).")
        }
    }
    
    func udpSocket(_ sock: GCDAsyncUdpSocket, didReceive data: Data, fromAddress address: Data, withFilterContext filterContext: Any?) {
        guard connected == false else { return }
        connected = true
        
        // Stop listening to broadcast
        broadcastSocket.close()
        
        // Use IP address to create WebSocket connections
        initWebsocketConnections(ip: String(data: data, encoding: .utf8)!)
    }
    
    func initWebsocketConnections(ip: String) {
        rotationSocket = WebSocket(url: URL(string: "ws://\(ip):9001/M_Rotation")!)
        inputSocket = WebSocket(url: URL(string: "ws://\(ip):9001/M_Input")!)
        
        let upBlock = { () -> Void in
            self.sendJSON(json: JSON(JSON_CONSTANTS.UP_PRESS))
            
            if UIDevice.current.orientation == UIDeviceOrientation.portrait {
                self.resetConnection()
            }
        }
        
        let downBlock = { () -> Void in
            self.sendJSON(json: JSON(JSON_CONSTANTS.DOWN_PRESS))

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
            self.sendJSON(json: JSON(JSON_CONSTANTS.ORIENTATION_LANDSCAPE))
        } else {
            self.portraitView.isHidden = false
            self.sendJSON(json: JSON(JSON_CONSTANTS.ORIENTATION_PORTRAIT))
        }
    }
    
    func sendText(text: String) {
        var textJSONified = JSON_CONSTANTS.SEND_TEXT
        textJSONified["text"] = text
        
        sendJSON(json: JSON(textJSONified))
    }
    
    func sendStrokes(strokes: [[[Float]]]) {
        var strokesJSONified = JSON_CONSTANTS.SEND_STROKES
        strokesJSONified["strokes"] = JSON(strokes).rawString()
        
        sendJSON(json: JSON(strokesJSONified))
    }
    
    func sendJSON(json: JSON ) {
        guard connected == true else { return }
        self.inputSocket.write(string: json.rawString()!)
    }
    
    override func preferredScreenEdgesDeferringSystemGestures() -> UIRectEdge {
        return [.bottom,.top]
    }
}
