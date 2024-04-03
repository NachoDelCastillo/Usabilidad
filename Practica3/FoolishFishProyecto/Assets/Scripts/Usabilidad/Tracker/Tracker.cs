using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    #region Singleton
    private static Tracker instance;
    public static Tracker Instance { get { return instance; } }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // endregion Singleton

    #region Properties
    IPersistence persistenceObject;
    List<ITrackerAsset> activeTrackers;
    #endregion // endregion Properties

    #region Methods
    public void Init()
    {
            // genariamos el id de Sesion Unico , supongo que usando SHA 256 u otro metodo de seguridad
            // Lectura de las propiedades de los eventos a partir de un archivo de configuracion json que indique las cosas de cada uno
            //Tambien se organiza como se va a serializar la informacion

    }

    public void End()
    {
        
        // Si hay objetos que siguen persistiendose , aqui habria que recorrerlos
        // y hacer un flush

        // Se borra la instancia del singleton
            if (instance_ != nullptr) {
                 delete instance_;
                instance_ = nullptr;
        }

    }
    public void Update(float dt) {

        // habria que tener un contador par ir haciendo flush/actualizació /escritura de los eventos
        // timer += Time.deltaTime;

        // IDEA de actualizacion de tracker
        //         Si es hora de hacer flush de los objetos a persistir:
        // Para cada objeto persistente en la lista :
        //     Llamar a la atualización / flush /escritura de los eventos
        // Reiniciar contador de tiempo

        //Actualizacion de eventos periodicos como la progresion? nose es idea -> progressionevent-> update()
    }

    public void TrackEvent()
    {

    }


    //Si los eventos se crean desde aqui habria que tener una factoría de eventos, por ejemplo:
    //Si se hace con enum seria llamando solo a tracker
    // create ProgressionEvent
    // create ResourceEvent
    // create TrackerEvent


    #endregion // endregion Methods
}
