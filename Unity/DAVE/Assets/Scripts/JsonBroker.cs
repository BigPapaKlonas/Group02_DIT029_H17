using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class JsonBroker {

  private QuickType.JSON json;
  private Button uplButton;

  public JsonBroker(string nJson){

    QuickType.JSON parsed = QuickType.JsonHelper.ParseToClass(nJson);
    this.json = parsed;
    uplButton = GameObject.Find("UploadBtn").GetComponent<Button>();

  }

  public void Render(){
    RenderSystemBoxes();
    RenderMessages();
  }

  private void RenderSystemBoxes(){
    uplButton.GetComponent<RenderSystemBoxes>().CreateSystemBoxes(this.json);
  }

  private void RenderMessages(){
    uplButton.GetComponent<StartMessages>().NewMessage(this.json);
  }

}
