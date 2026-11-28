using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020007AE RID: 1966
	public abstract class Rule
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06003F61 RID: 16225
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x00195F9A File Offset: 0x0019419A
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x00195FA4 File Offset: 0x001941A4
		public virtual Rule DeepCopy()
		{
			Rule rule = (Rule)Activator.CreateInstance(base.GetType());
			rule.keyword = this.keyword;
			rule.tag = this.tag;
			rule.requiredTag = this.requiredTag;
			if (this.constantConstraints != null)
			{
				rule.constantConstraints = this.constantConstraints.ToList<Rule.ConstantConstraint>();
			}
			return rule;
		}

		// Token: 0x06003F64 RID: 16228
		public abstract string Generate();

		// Token: 0x06003F65 RID: 16229 RVA: 0x00196000 File Offset: 0x00194200
		public virtual void Init()
		{
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x00196004 File Offset: 0x00194204
		public void AddConstantConstraint(string key, string value, Rule.ConstantConstraint.Type type)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstantConstraint
			{
				key = key,
				value = value,
				type = type
			});
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x00196050 File Offset: 0x00194250
		public void AddConstantConstraint(string key, string value, string op)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(op);
			Rule.ConstantConstraint.Type type;
			if (num <= 1678027958U)
			{
				if (num <= 957132539U)
				{
					if (num != 284975636U)
					{
						if (num == 957132539U)
						{
							if (op == "<")
							{
								type = Rule.ConstantConstraint.Type.Less;
								goto IL_123;
							}
						}
					}
					else if (op == ">=")
					{
						type = Rule.ConstantConstraint.Type.GreaterOrEqual;
						goto IL_123;
					}
				}
				else if (num != 990687777U)
				{
					if (num == 1678027958U)
					{
						if (op == "[less_than]")
						{
							type = Rule.ConstantConstraint.Type.Less;
							goto IL_123;
						}
					}
				}
				else if (op == ">")
				{
					type = Rule.ConstantConstraint.Type.Greater;
					goto IL_123;
				}
			}
			else if (num <= 2431966415U)
			{
				if (num != 2428715011U)
				{
					if (num == 2431966415U)
					{
						if (op == "==")
						{
							type = Rule.ConstantConstraint.Type.Equal;
							goto IL_123;
						}
					}
				}
				else if (op == "!=")
				{
					type = Rule.ConstantConstraint.Type.NotEqual;
					goto IL_123;
				}
			}
			else if (num != 2499223986U)
			{
				if (num == 4183126053U)
				{
					if (op == "[greater_than]")
					{
						type = Rule.ConstantConstraint.Type.Greater;
						goto IL_123;
					}
				}
			}
			else if (op == "<=")
			{
				type = Rule.ConstantConstraint.Type.LessOrEqual;
				goto IL_123;
			}
			type = Rule.ConstantConstraint.Type.Equal;
			Log.Error("Unknown ConstantConstraint type: " + op);
			IL_123:
			this.AddConstantConstraint(key, value, type);
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x0019618C File Offset: 0x0019438C
		public bool ValidateConstraints(Dictionary<string, string> constraints)
		{
			bool result = true;
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = this.constantConstraints[i];
					string text = (constraints != null) ? constraints.TryGetValue(constantConstraint.key, "") : "";
					float num = 0f;
					float num2 = 0f;
					bool flag = !text.NullOrEmpty() && !constantConstraint.value.NullOrEmpty() && float.TryParse(text, out num) && float.TryParse(constantConstraint.value, out num2);
					bool flag2;
					switch (constantConstraint.type)
					{
					case Rule.ConstantConstraint.Type.Equal:
						flag2 = text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.NotEqual:
						flag2 = !text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.Less:
						flag2 = (flag && num < num2);
						break;
					case Rule.ConstantConstraint.Type.Greater:
						flag2 = (flag && num > num2);
						break;
					case Rule.ConstantConstraint.Type.LessOrEqual:
						flag2 = (flag && num <= num2);
						break;
					case Rule.ConstantConstraint.Type.GreaterOrEqual:
						flag2 = (flag && num >= num2);
						break;
					default:
						Log.Error("Unknown ConstantConstraint type: " + constantConstraint.type.ToString());
						flag2 = false;
						break;
					}
					if (!flag2)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x040029F1 RID: 10737
		[MayTranslate]
		public string keyword;

		// Token: 0x040029F2 RID: 10738
		[NoTranslate]
		public string tag;

		// Token: 0x040029F3 RID: 10739
		[NoTranslate]
		public string requiredTag;

		// Token: 0x040029F4 RID: 10740
		public int? usesLimit;

		// Token: 0x040029F5 RID: 10741
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x02002AC1 RID: 10945
		public struct ConstantConstraint
		{
			// Token: 0x0400AF7D RID: 44925
			[MayTranslate]
			public string key;

			// Token: 0x0400AF7E RID: 44926
			[MayTranslate]
			public string value;

			// Token: 0x0400AF7F RID: 44927
			public Rule.ConstantConstraint.Type type;

			// Token: 0x02003F53 RID: 16211
			public enum Type
			{
				// Token: 0x0400F9A4 RID: 63908
				Equal,
				// Token: 0x0400F9A5 RID: 63909
				NotEqual,
				// Token: 0x0400F9A6 RID: 63910
				Less,
				// Token: 0x0400F9A7 RID: 63911
				Greater,
				// Token: 0x0400F9A8 RID: 63912
				LessOrEqual,
				// Token: 0x0400F9A9 RID: 63913
				GreaterOrEqual
			}
		}
	}
}
