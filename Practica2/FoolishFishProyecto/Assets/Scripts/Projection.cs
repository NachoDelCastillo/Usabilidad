using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100; //Numero de pasos fisicos
    [SerializeField] private Transform _obstaclesParent;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    private void Start(){
        CreatePhysicsScene();
        //_simulationScene.isSubScene= true;
       
    }

    private void CreatePhysicsScene(){
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstaclesParent){
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    private void Update(){
        foreach (var item in _spawnedObjects){
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    public void SimulateTrajectory(FishMovement player){
        var ghostObj = Instantiate(player, player.transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.Jump();
        //Debug.LogError(ghostObj.GetComponent<Rigidbody2D>().velocity);
        _line.positionCount = _maxPhysicsFrameIterations;
       
        
        for (var i = 0; i < _maxPhysicsFrameIterations; i++){
            _physicsScene.Simulate(Time.fixedDeltaTime);
            //Debug.LogError("Position: "+ghostObj.transform.position);
            _line.SetPosition(i, ghostObj.transform.position);
        }
       
        Destroy(ghostObj.gameObject);
    }
}
