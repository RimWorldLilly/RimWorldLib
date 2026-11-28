using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000119 RID: 281
	public class BodyDef : Def
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x00035BFA File Offset: 0x00033DFA
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00035C02 File Offset: 0x00033E02
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00035C0C File Offset: 0x00033E0C
		public List<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			if (!this.cachedPartsByTag.ContainsKey(tag))
			{
				this.cachedPartsByTag[tag] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def.tags.Contains(tag))
					{
						this.cachedPartsByTag[tag].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByTag[tag];
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00035C8C File Offset: 0x00033E8C
		public List<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			if (!this.cachedPartsByDef.ContainsKey(def))
			{
				this.cachedPartsByDef[def] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def == def)
					{
						this.cachedPartsByDef[def].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByDef[def];
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00035D04 File Offset: 0x00033F04
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				if (this.AllParts[i].def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00035D48 File Offset: 0x00033F48
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00035D6C File Offset: 0x00033F6C
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00035DA1 File Offset: 0x00033FA1
		public override IEnumerable<string> ConfigErrors()
		{
			BodyDef.<ConfigErrors>d__14 <ConfigErrors>d__ = new BodyDef.<ConfigErrors>d__14(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00035DB4 File Offset: 0x00033FB4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00035E28 File Offset: 0x00034028
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + ((this != null) ? this.ToString() : null));
				return;
			}
			node.body = this;
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (Mathf.Abs(num) < 1E-05f)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				num = 0f;
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00035FD6 File Offset: 0x000341D6
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedPartsByTag.Clear();
			this.cachedPartsByDef.Clear();
		}

		// Token: 0x04000494 RID: 1172
		public BodyPartRecord corePart;

		// Token: 0x04000495 RID: 1173
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x04000496 RID: 1174
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;

		// Token: 0x04000497 RID: 1175
		[Unsaved(false)]
		public Dictionary<BodyPartTagDef, List<BodyPartRecord>> cachedPartsByTag = new Dictionary<BodyPartTagDef, List<BodyPartRecord>>();

		// Token: 0x04000498 RID: 1176
		[Unsaved(false)]
		public Dictionary<BodyPartDef, List<BodyPartRecord>> cachedPartsByDef = new Dictionary<BodyPartDef, List<BodyPartRecord>>();
	}
}
