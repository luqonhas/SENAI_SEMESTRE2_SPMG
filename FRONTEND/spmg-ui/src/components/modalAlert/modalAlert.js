import React, { Component } from "react";
import "../../assets/css/modalAlert.css";

export default class ModalAlert extends Component {
  constructor(props) {
    super(props);

    this.state = {
        active : true
    }
  }

  onDivClick() {    
    this.setState({active : false});

    this.props.onClick();
  }

  componentWillReceiveProps()
  {
    this.setState({active : true});
  }

  render() {
    return (
      <div onClick={() => this.onDivClick()} id={this.state.active ? "modal-alert-background" : "modal-alert-none"}>
        <div id="modal-alert-item">
          <span>{this.props.titulo}</span>
          <p>{this.props.descricao}</p>
        </div>
      </div>
    );
  }
}