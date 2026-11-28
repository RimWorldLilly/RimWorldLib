using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200053A RID: 1338
	public class CompAttachBase : ThingComp
	{
		// Token: 0x0600275C RID: 10076 RVA: 0x000F62B0 File Offset: 0x000F44B0
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000F62F8 File Offset: 0x000F44F8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000F6340 File Offset: 0x000F4540
		public override string CompInspectStringExtra()
		{
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return null;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000F6398 File Offset: 0x000F4598
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000F63E5 File Offset: 0x000F45E5
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000F63F1 File Offset: 0x000F45F1
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000F6412 File Offset: 0x000F4612
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x04001C44 RID: 7236
		public List<AttachableThing> attachments;
	}
}
