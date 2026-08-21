using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ManaMagic.Core.Events.OpCodes;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events
{
    public static class EventUtil
    {
        private static Dictionary<EventOpCodeType, Type>? EventOpCodeTypeDictionary = null;

        public static IReadOnlyDictionary<EventOpCodeType, Type> GetEventOpCodeDictionary()
        {
            if (EventUtil.EventOpCodeTypeDictionary == null)
            {
                List<Type> typeList = Assembly.GetExecutingAssembly()
                                              .GetTypes()
                                              .Where(p => !string.IsNullOrWhiteSpace(p.Namespace) &&
                                                          p.Namespace.StartsWith("ManaMagic.Core.Events.OpCodes", StringComparison.Ordinal) &&
                                                          p.IsAssignableTo(typeof(EventOpCode)) &&
                                                          !p.IsAbstract)
                                              .ToList();

                EventUtil.EventOpCodeTypeDictionary = new Dictionary<EventOpCodeType, Type>();
                foreach (Type type in typeList)
                {
                    EventOpCodeTypeAttribute? attribute = EventUtil.GetEventOpCodeTypeAttribute(type);
                    if (attribute == null)
                    {
                        ThrowHelper.ThrowInvalidOperationException($"Could not find EventOpCodeTypeAttribute for {type.Name}");
                    }
                    EventUtil.EventOpCodeTypeDictionary.Add(attribute.OperationCode, type);

                    if (attribute.OperationCode == EventOpCodeType.UseDoor1)
                    {
                    }
                }
                // UseDoor, CallEvent and JumpToEvent have multiple op-codes for a single class, so we gotta add those manually.
                Type useDoorType = EventUtil.EventOpCodeTypeDictionary[EventOpCodeType.UseDoor1];
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.UseDoor2, useDoorType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.UseDoor3, useDoorType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.UseDoor4, useDoorType);

                Type callEventType = EventUtil.EventOpCodeTypeDictionary[EventOpCodeType.CallEvent1];
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent2, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent3, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent4, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent5, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent6, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent7, callEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.CallEvent8, callEventType);

                Type jumpEventType = EventUtil.EventOpCodeTypeDictionary[EventOpCodeType.JumpToEvent1];
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent2, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent3, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent4, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent5, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent6, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent7, jumpEventType);
                EventUtil.EventOpCodeTypeDictionary.Add(EventOpCodeType.JumpToEvent8, jumpEventType);
            }
            return EventUtil.EventOpCodeTypeDictionary;
        }

        public static Type GetEventOpCodeClassType(EventOpCodeType type)
        {
            return EventUtil.GetEventOpCodeDictionary()[type];
        }

        public static EventOpCodeType GetEventOpCodeType(Type type)
        {
            EventOpCodeTypeAttribute? attribute = EventUtil.GetEventOpCodeTypeAttribute(type);
            if (attribute == null)
            {
                ThrowHelper.ThrowArgumentException("Attribute not found.", nameof(type));
            }
            return attribute.OperationCode;
        }

        public static string GetEventDescription(Type type)
        {
            DescriptionAttribute? attribute = EventUtil.GetDescriptionAttribute(type);
            return attribute != null ? attribute.Description : string.Empty;
        }

        public static EventOpCodeCommandTypeAttribute? GetEventOpCodeCommandTypeAttribute(Type type)
        {
            object[] attributeArray = type.GetCustomAttributes(typeof(EventOpCodeCommandTypeAttribute), true);
            if (attributeArray != null && attributeArray.Length > 0 && attributeArray[0] is EventOpCodeCommandTypeAttribute attribute)
            {
                return attribute;
            }
            return null;
        }

        public static EventOpCodeTypeAttribute? GetEventOpCodeTypeAttribute(Type type)
        {
            object[] attributeArray = type.GetCustomAttributes(typeof(EventOpCodeTypeAttribute), true);
            if (attributeArray != null && attributeArray.Length > 0 && attributeArray[0] is EventOpCodeTypeAttribute attribute)
            {
                return attribute;
            }
            return null;
        }

        public static DescriptionAttribute? GetDescriptionAttribute(Type type)
        {
            object[] attributeArray = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributeArray != null && attributeArray.Length > 0 && attributeArray[0] is DescriptionAttribute attribute)
            {
                return attribute;
            }
            return null;
        }
    }
}