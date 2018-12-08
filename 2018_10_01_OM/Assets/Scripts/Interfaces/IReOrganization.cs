using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrganizationalModel.Behaviors
{

    interface IReOrganization<T, U>
    {


        void IResetProperties();

        void IAgentStatesBehaviorConditions();
        void ISignalReceiverPassiveBehaviorConditions();
        void ISignalEmmiterBehaviorConditions();
        void ISignalReceiverBehaviorConditions();
        void IRulesDiscrete();
        void IRulesContinuous();
        void IChangeStateToSignalReceiverPassive();
        void IChangeStateToSignalEmmiter();
        void IChangeStateToDeactivated();
    }
}
