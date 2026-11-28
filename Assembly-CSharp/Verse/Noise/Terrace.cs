using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000781 RID: 1921
	public class Terrace : ModuleBase
	{
		// Token: 0x06003C72 RID: 15474 RVA: 0x0017DA7D File Offset: 0x0017BC7D
		public Terrace() : base(1)
		{
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0017DA91 File Offset: 0x0017BC91
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0017DAAE File Offset: 0x0017BCAE
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003C75 RID: 15477 RVA: 0x0017DAD2 File Offset: 0x0017BCD2
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x0017DADF File Offset: 0x0017BCDF
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003C77 RID: 15479 RVA: 0x0017DAE7 File Offset: 0x0017BCE7
		// (set) Token: 0x06003C78 RID: 15480 RVA: 0x0017DAEF File Offset: 0x0017BCEF
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x0017DAF8 File Offset: 0x0017BCF8
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0017DB49 File Offset: 0x0017BD49
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x0017DB58 File Offset: 0x0017BD58
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0017DBB4 File Offset: 0x0017BDB4
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num])
			{
				num++;
			}
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num3];
			}
			double num4 = this.m_data[num2];
			double num5 = this.m_data[num3];
			double num6 = (value - num4) / (num5 - num4);
			if (this.m_inverted)
			{
				num6 = 1.0 - num6;
				double num7 = num4;
				num4 = num5;
				num5 = num7;
			}
			num6 *= num6;
			return Utils.InterpolateLinear(num4, num5, num6);
		}

		// Token: 0x0400274F RID: 10063
		private List<double> m_data = new List<double>();

		// Token: 0x04002750 RID: 10064
		private bool m_inverted;
	}
}
