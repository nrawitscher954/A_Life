using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrganizationalModel.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam> GameObject
    interface IOrganization<T>
    {

      //  void IChangeStateToSignalReceiver(T data);
      //  void IChangeStateToSignalReceiverPassive(T data);
      //  void IChangeStateToFreezed(T data);
     //   void IChangeStateToSignalEmmiter(T data);




        void ISignalReceiverConditions();
        void ISignalReceiverPassiveConditions();
        void ISignalEmmiterConditions();

        void IAgentBehaviorConditions();

    }
}
