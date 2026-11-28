using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000588 RID: 1416
	public class RealtimeMoteList
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x000FF988 File Offset: 0x000FDB88
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000FF995 File Offset: 0x000FDB95
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000FF9A3 File Offset: 0x000FDBA3
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000FF9B4 File Offset: 0x000FDBB4
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x04001CF5 RID: 7413
		public List<Mote> allMotes = new List<Mote>();
	}
}
