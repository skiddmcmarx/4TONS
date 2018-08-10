using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager{

    void subscribeToEvents();
    void unsubscribeFromEvents();
    void initialize();
    void update();
    void endScene();
    void changeScene(SceneInfo sceneChangeInfo);
    void load(int sceneIndex);
    void joinPlayer(JoinPlayerInfo joinPlayerInfo);
    void leavePlayer(LeavePlayerInfo leaverPlayerInfo);

}
