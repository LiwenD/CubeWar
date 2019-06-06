/*
 * Clean摘抄自：http://wiki.unity3d.com/index.php/Finite_State_Machine
 * 具体细节根据需求再改 2019.6.6
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private List<FSMState> states;

    // The only way one can change the state of the FSM is by performing a transition
    // Don't change the CurrentState directly
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }

    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }
        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }
	
        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();

                currentState = state;

                currentState.DoBeforeEntering();
                break;
            }
        }

    } // PerformTransition()

} //class FSMSystem
