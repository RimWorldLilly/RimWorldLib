using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200034A RID: 842
	public class PatchOperation
	{
		// Token: 0x06001A2F RID: 6703 RVA: 0x0009CD50 File Offset: 0x0009AF50
		public bool Apply(XmlDocument xml)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(base.GetType().FullName + " Worker");
			}
			bool flag = this.ApplyWorker(xml);
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0009CDC9 File Offset: 0x0009AFC9
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail");
			return false;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0009CDD8 File Offset: 0x0009AFD8
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text);
			}
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0009CE1F File Offset: 0x0009B01F
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x04001559 RID: 5465
		public string sourceFile;

		// Token: 0x0400155A RID: 5466
		private bool neverSucceeded = true;

		// Token: 0x0400155B RID: 5467
		private PatchOperation.Success success;

		// Token: 0x02002685 RID: 9861
		private enum Success
		{
			// Token: 0x04009DD2 RID: 40402
			Normal,
			// Token: 0x04009DD3 RID: 40403
			Invert,
			// Token: 0x04009DD4 RID: 40404
			Always,
			// Token: 0x04009DD5 RID: 40405
			Never
		}
	}
}
