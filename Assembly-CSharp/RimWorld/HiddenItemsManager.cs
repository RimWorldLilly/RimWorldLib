using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001028 RID: 4136
	public sealed class HiddenItemsManager : IExposable
	{
		// Token: 0x0600682E RID: 26670 RVA: 0x0024D886 File Offset: 0x0024BA86
		public void SetDiscovered(ThingDef def)
		{
			if (!this.hiddenItemDefs.ContainsKey(def))
			{
				return;
			}
			this.hiddenItemDefs[def] = false;
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x0024D8A4 File Offset: 0x0024BAA4
		public bool Hidden(ThingDef def)
		{
			bool flag;
			return this.hiddenItemDefs.TryGetValue(def, out flag) && flag;
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x0024D8C4 File Offset: 0x0024BAC4
		public HiddenItemsManager()
		{
			this.hiddenItemDefs = new Dictionary<ThingDef, bool>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.hiddenWhileUndiscovered)
				{
					this.hiddenItemDefs.Add(thingDef, true);
				}
			}
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x0024D930 File Offset: 0x0024BB30
		public void ClearHiddenDefs()
		{
			this.hiddenItemDefs.Clear();
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x0024D940 File Offset: 0x0024BB40
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef, bool>(ref this.hiddenItemDefs, "hiddenItemDefs", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Dictionary<ThingDef, bool> dictionary = new Dictionary<ThingDef, bool>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.hiddenWhileUndiscovered)
					{
						dictionary.Add(thingDef, true);
					}
				}
				foreach (KeyValuePair<ThingDef, bool> keyValuePair in this.hiddenItemDefs)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				this.hiddenItemDefs = dictionary;
			}
		}

		// Token: 0x0400425E RID: 16990
		private Dictionary<ThingDef, bool> hiddenItemDefs;
	}
}
