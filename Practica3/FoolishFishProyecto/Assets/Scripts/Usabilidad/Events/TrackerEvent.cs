using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerEvent : MonoBehaviour
{
    public enum EVENT_TYPE { }; 
    // Enum -> String (Comodidad)
    const string eventTypes[] // Asi podemos obtener su nombre directamente y no solo su posicion
  
    //si no se inlcuyen hay que incluir CSVFields


protected:

	int timestamp_;
	std::string id_;
	EventType eventType_;
	// uint16_t maskBits_;  -> por si no recogemos todos los eventos y queremos ver cual es trackeable , aunque no es seguro si lo necesitamos



    //Ya sea en el start o en una constructora tenemos que crear este objeto
    //TrackerEvent(int timestamp, const std::string& id, EventType eventType)
    //TrackerEvent::TrackerEvent(int timestamp, const std::string& id, ::EventType eventType)
    // {
	// timestamp_ = timestamp;
	// id_ = id;
	// eventType_ = eventType;}
    
    void toCSV(std::unordered_map<CSVFields, std::string>& eventCSV) {

	//eventCSV[CSVFields::Id] = id_;
	//eventCSV[CSVFields::Time] = string(timestamp_);
	//eventCSV[CSVFields::EventType] = eventTypes[(int)eventType_];

    }
        

    string ToJSON()
    {
            //objetoJson json
            //json["Eventype"]= eventTypes[(int)eventType_] 
            //json["id"]= id;
            //json["TimeStamp"] = timestamp_;
            //return json
        return "";
    } 
    // EventType getType() 
    // bool sePuedeTrackearEVento(mascara de bits o algo?)
    void DestroyEvent(TrackerEvent* event) {
	        Destroy(event) ; 

    }
	
}
