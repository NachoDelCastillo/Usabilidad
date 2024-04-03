using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSVSerializer : MonoBehaviour, ISerializer
{
    private string interfix = "\n";
    private string separator = "\t";
    private string prefix = "";
    private Dictionary<CSVFields, string> eventCSV = new Dictionary<CSVFields, string>();

    private enum CSVFields
    {
        //Puede ser que sea obligatorio incluir -> ID / Time /EventType
        // Nombres de lo que queramos analizar como por ejemplo -> numero de playaformas , de saltos ..etc
         LAST
        
    }

    private string[] CSVPrefixes = {
        // nombres de los campos específicos 
        //Puede ser que sea obligatorio incluir -> "ID", "Time", "EventType"
    };

    void Start()
    {
        InitializePrefix();
    }

    private void InitializePrefix()
    {
        for (int i = 0; i < (int)CSVFields.LAST; i++)
        {
            if (i < (int)CSVFields.LAST - 1)
            {
                prefix += CSVPrefixes[i] + separator;
            }
            else
            {
                prefix += CSVPrefixes[i];
            }
            eventCSV[(CSVFields)i] = "";
        }
        prefix += interfix;
    }


    //Idea para metodo serialize
   
    // InicializarPrefijo()
    // LimpiarEventCSV()
    // Convertir evento a formato CSV
    // Crear cadena CSV vacía
    // Para cada campo en CSVFields:
    //     Si no es el último campo:
    //         Agregar valor del campo al CSV con separador
    //     Sino:
    //         Agregar valor del campo al CSV sin separador
    // Devolver cadena CSV
    public string Serialize(TrackerEvent trackerEvent)
    
    {
        ClearEventCSV();
        trackerEvent.ToCSV(eventCSV);

        string csvString = "";

        for (int i = 0; i < (int)CSVFields.LAST; i++)
        {
            if (i < (int)CSVFields.LAST - 1)
            {
                csvString += eventCSV[(CSVFields)i] + separator;
            }
            else
            {
                csvString += eventCSV[(CSVFields)i];
            }
        }
        return csvString;
    }

    public string GetPrefix()
    {
        return prefix;
    }

    public string GetInterfix()
    {
        return interfix;
    }

    public string GetSufix()
    {
        return sufix;
    }

    private void ClearEventCSV()
    {
        for (int i = 0; i < (int)CSVFields.LAST; i++)
        {
            eventCSV[(CSVFields)i] = "";
        }
        eventCSV.Clear();
    }
}

