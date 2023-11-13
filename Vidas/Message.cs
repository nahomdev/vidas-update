﻿using System;
using System.Collections.Generic;
using System.Text;
using Vidas;

namespace Vidas
{
    public class Message
    {
        private const string MSH = "H";
        private const int MSH_MSG_CONTROL_ID = 10;

        private List<Segment> segments;

        public Message()
        {
            Initialize();
        }

        public void Initialize()
        {
            segments = new List<Segment>();
        }

        protected Segment Header()
        {
            if (segments.Count == 0 || segments[0].Name != MSH)
            {
                return null;
            }
            return segments[0];
        }

        public string MessageControlId()
        {
            Segment msh = Header();
            if (msh == null) return String.Empty;
            return msh.Field(MSH_MSG_CONTROL_ID);
        }

        public void Add(Segment segment)
        {
            if (!String.IsNullOrEmpty(segment.Name) && segment.Name.Length == 3)
            {
                segments.Add(segment);
            }
        }

        public void DeSerializeMessage(string msg)
        {
            Initialize();

            char[] separator = { '\r' };
            var tokens = msg.Split(separator, StringSplitOptions.None);

            foreach (var item in tokens)
            {
                var segment = new Segment();
                segment.DeSerializeSegment(item.Trim('\n'));
                Add(segment);
            }
        }

        public string SerializeMessage()
        {
            var builder = new StringBuilder();
            char[] separators = { '\r', '\n' };

            foreach (var segment in segments)
            {
                builder.Append(segment.SerializeSegment());
                builder.Append("\r\n");
            }
            return builder.ToString().TrimEnd(separators);
        }
    }
}
