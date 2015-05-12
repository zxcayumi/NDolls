using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询项-条件组(组内条件为OR关系)
    /// </summary>
    public class ConditionOrGroup : Item
    {
        private List<Item> groupConditions = new List<Item>();

        public ConditionOrGroup()
        {
            this.ItemType = Entity.ItemType.ConditionGroup;
        }

        /// <summary>
        /// 添加条件项
        /// </summary>
        /// <param name="item"></param>
        public void AddCondition(Item item)
        {
            groupConditions.Add(item);
        }

        public override void LoadParameters(StringBuilder sb, List<System.Data.Common.DbParameter> pars, JoinType joinType)
        {
            if (sb.Length > 1)
            {
                sb.Append(" " + joinType.ToString() + " ");
            }

            sb.Append("(");
            foreach (Item item in GroupConditions)
            {
                if(item is ConditionItem)
                {
                    item.LoadParameters(sb, pars, JoinType.OR);
                }
                else if (item is ConditionAndGroup)
                {
                    ConditionAndGroup group = item as ConditionAndGroup;
                    group.IsSubGroup = true;
                    group.LoadParameters(sb, pars, JoinType.OR);
                }
            }
            sb.Append(")");
        }

        /// <summary>
        /// 是否为嵌套组（在别的Group内的子Group）
        /// </summary>
        public Boolean IsSubGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 组条件项集合
        /// </summary>
        public List<Item> GroupConditions
        {
            get { return groupConditions; }
            set { groupConditions = value; }
        }
    }
}
