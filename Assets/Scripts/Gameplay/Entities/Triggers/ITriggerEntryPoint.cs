using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Must be implemented by objects that should react to a trigger button */
public interface ITriggerEntryPoint {

    void RegisterButton(Button btn);

    void TriggerAction();

    bool CheckAllButtonsValid();

    void LockAllButtons();
}
