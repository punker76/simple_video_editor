﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorWPF
{
	public class TimelineEvent
	{
		public string name;

		public double startTime;
		public double endTime;
		public Object data;

		public TimelineEvent(string name, double startTime, double endTime, Object data)
		{
			this.name = name;
			this.startTime = startTime;
			this.endTime = endTime;
			this.data = data;
		}
	}
}
