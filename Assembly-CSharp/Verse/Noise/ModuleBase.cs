using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000767 RID: 1895
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003BCB RID: 15307 RVA: 0x0017BC14 File Offset: 0x00179E14
		public int SourceModuleCount
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Length;
				}
				return 0;
			}
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x0017BC28 File Offset: 0x00179E28
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000BAA RID: 2986
		public virtual ModuleBase this[int index]
		{
			get
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (this.modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return this.modules[index];
			}
			set
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				this.modules[index] = value;
			}
		}

		// Token: 0x06003BCF RID: 15311
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0017BCAC File Offset: 0x00179EAC
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0017BCCC File Offset: 0x00179ECC
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0017BCEA File Offset: 0x00179EEA
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x0017BD08 File Offset: 0x00179F08
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x0017BD10 File Offset: 0x00179F10
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x0017BD2C File Offset: 0x00179F2C
		protected virtual bool Disposing()
		{
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					this.modules[i].Dispose();
					this.modules[i] = null;
				}
				this.modules = null;
			}
			return true;
		}

		// Token: 0x04002715 RID: 10005
		protected ModuleBase[] modules;

		// Token: 0x04002716 RID: 10006
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
