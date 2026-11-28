using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200076F RID: 1903
	public class Curve : ModuleBase
	{
		// Token: 0x06003C11 RID: 15377 RVA: 0x0017CC3F File Offset: 0x0017AE3F
		public Curve() : base(1)
		{
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0017CC53 File Offset: 0x0017AE53
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06003C13 RID: 15379 RVA: 0x0017CC70 File Offset: 0x0017AE70
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06003C14 RID: 15380 RVA: 0x0017CC7D File Offset: 0x0017AE7D
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0017CC88 File Offset: 0x0017AE88
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0017CCE2 File Offset: 0x0017AEE2
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x0017CCF0 File Offset: 0x0017AEF0
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num].Key)
			{
				num++;
			}
			int index = Mathf.Clamp(num - 2, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(num + 1, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num2].Value;
			}
			double key = this.m_data[num2].Key;
			double key2 = this.m_data[num3].Key;
			double position = (value - key) / (key2 - key);
			return Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num2].Value, this.m_data[num3].Value, this.m_data[index2].Value, position);
		}

		// Token: 0x04002733 RID: 10035
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
