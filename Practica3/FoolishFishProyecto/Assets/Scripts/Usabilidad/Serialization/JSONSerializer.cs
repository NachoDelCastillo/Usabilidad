using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class JSONSerializer : MonoBehaviour, ISerializer
{

    //ESTO ES UN EJEMPLO , Nuestra distribuccion de prefijos sufijos e interfijos la tenemos que decidir

    private string prefix = "{\n\t\"events\": [\n";
    private string interfix = ",\n";
    private string sufix = "\n\t]\n}";

    public string Serialize(TrackerEvent trackerEvent)
    {

    //IDEA DE SERIALIZACION EN JSON -> el codigo de debajo pero explicado

    // Inicializar la cadena JSON con el prefijo
    // cadenaJSON = "{\n\t\"events\": [\n"

    //Serializar el evento del tracker a formato JSON y añadirlo a la cadena JSON
    // cadenaEvento = trackerEvent.ToJson()

    //Indentar la cadena del evento
    // cadenaEventoIndentada = IndentarCadena(cadenaEvento)

    //Concatenar la cadena del evento indentada a la cadena JSON
    // cadenaJSON = cadenaJSON + "\t\t" + cadenaEventoIndentada

    //Añadir el interfijo al final de la cadena JSON
    // cadenaJSON = cadenaJSON + ",\n"

    //Añadir el sufijo al final de la cadena JSON
    // cadenaJSON = cadenaJSON + "\n\t]\n}"

    //Devolver la cadena JSON completa
    // devolver cadenaJSON



    string stringEvent = trackerEvent.ToJson();

    //ESTO ES UN EJEMPLO, la indentacion creo que la tendremos que decidir
    string indentString = "\t\t";
    int pos = 0;
    while (pos != -1) //Lectura de todo el Json (stringevent)
    {
        pos = stringEvent.IndexOf('\n', pos + 1);  // Buscamos en la nueva linea a leer el nuevo caracter
        if (pos != -1)
        {
            stringEvent = stringEvent.Insert(pos + 1, indentString);  // Insertamos una indentacion en la nueva linea
            pos += indentString.Length + 1;  // Avanzamos tanto como indentacion hayamos puesto
        }
    }

    stringEvent = stringEvent.Insert(0, indentString); //Ultima indentacion (por asegurar)

    return stringEvent;
   
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
}