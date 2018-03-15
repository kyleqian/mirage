import React from 'react';
import { Button, Dimensions, StyleSheet, Text, View } from 'react-native';

import RNDraw from 'rn-draw'
  

const {height, width} = Dimensions.get('window');

let ws = new WebSocket('ws://mirage-relay-server.herokuapp.com')

// Add ability to space w/ double tab 

ws.onmessage = (e) => {
  // a message was received
  console.log(e.data);
};

ws.onerror = (e) => {
  // an error occurred
  console.log(e.message);
};

ws.onclose = (e) => {
  // connection closed
  console.log(e.code, e.reason);
};

export default class App extends React.Component {
  constructor(){
    super()
    this.state = { xCoords: [], 
      yCoords: [], recognized: ''
    }
  }

  _printStrokes = (currentPoints) => { 
    console.log(currentPoints)

    let xCoord = []
    let yCoord = []

    currentPoints.map((val, index) => {
      xCoord.push(val.x)
      yCoord.push(val.y)
    })

    console.log(xCoord)
    console.log(yCoord)

    let xCoords = this.state.xCoords.slice()
    let yCoords = this.state.yCoords.slice()
    xCoords.push(xCoord)
    yCoords.push(yCoord)

    this.setState({ ...this.state, xCoords: xCoords, yCoords: yCoords })
  }

  _getResults = () => {
    let trace = [] 
    this.state.xCoords.map((val, index) => {
      let stroke = [] 
      stroke.push(this.state.xCoords[index])
      stroke.push(this.state.yCoords[index])
      trace.push(stroke)
    })
    
    console.log(trace)
    let data = JSON.stringify({
      "options": "enable_pre_space",
      "requests": [{
          "writing_guide": {
              "writing_area_width": width || undefined,
              "writing_area_height": height || undefined
          },
          "ink": trace,
          "language": "en"
      }]
    })
    let foundText = ''
    fetch("https://www.google.com/inputtools/request?ime=handwriting&app=mobilesearch&cs=1&oe=UTF-8", {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'content-type': 'application/json',
      },
      body: data
      })
        .then((response) => {
          console.log(response)
          const text = response._bodyText 
          const js = JSON.parse(text)
          foundText = js[1][0][1]
          //Clear the state for the next letter
          this.setState({xCoords: [], yCoords: [], recognized: foundText})
          console.log(js[1][0][1])
          console.log("opening socket")  
          const firstLetter = js[1][0][1][0]      
          ws.onopen = () => {
            // connection opened
            console.log("sending a message")
            const message = "broadcast: " + firstLetter
            
            if (firstLetter === '..' || firstLetter === ':'){
              console.log('DOUBLE_TAP')
              ws.send('broadcast: SPACE')
            }
            else if (firstLetter === '...'){
              console.log('TRIPLE_TAP')
              ws.send('broadcast: END')
            }
            else{
              ws.send(message) // send a message
            }
          }; 
          ws.onopen()
          this._clear()
        })
        .catch((error) => {
          console.error(error)
        })
  }


  render() {
    return (
        <RNDraw
          containerStyle={{backgroundColor: 'rgba(0,0,0,0.01)'}}
          rewind={ (undo) => this._undo = undo}
          clear={(clear) => this._clear = clear} 
          color={'#000000'}
          strokeWidth={4}
          strokes={this._printStrokes}
          getLetter={this._getResults}
          myText={this.state.recognized}
        />
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
