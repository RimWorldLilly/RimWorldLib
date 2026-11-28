using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000356 RID: 854
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06001A4A RID: 6730 RVA: 0x0009D594 File Offset: 0x0009B794
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0009D5F8 File Offset: 0x0009B7F8
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0009D608 File Offset: 0x0009B808
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				string str = text;
				string str2 = ", lastFailedOperation=";
				PatchOperation patchOperation = this.lastFailedOperation;
				text = str + str2 + ((patchOperation != null) ? patchOperation.ToString() : null);
			}
			return text + ")";
		}

		// Token: 0x04001567 RID: 5479
		private List<PatchOperation> operations;

		// Token: 0x04001568 RID: 5480
		private PatchOperation lastFailedOperation;
	}
}
