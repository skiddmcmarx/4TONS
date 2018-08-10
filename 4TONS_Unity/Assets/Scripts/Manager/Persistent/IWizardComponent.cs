using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWizardComponent
{
    void subscribeToEvents();
    void unsubscribeFromEvents();
    void initialize(PlayerBehaviours playerBehaviours);
    void update();
    void endScene();

}

