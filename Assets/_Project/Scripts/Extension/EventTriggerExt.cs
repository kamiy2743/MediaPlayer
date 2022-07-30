using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MediaPlayer
{
    public static class EventTriggerExt
    {
        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType eventTriggerType, UnityAction call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener((e) => call.Invoke());
            eventTrigger.triggers.Add(entry);
        }

        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType eventTriggerType, UnityAction<PointerEventData> call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener((e) => call.Invoke((PointerEventData)e));
            eventTrigger.triggers.Add(entry);
        }
    }
}