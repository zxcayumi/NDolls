using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询项-条件组(组内条件为OR关系)
    /// </summary>
    public class ConditionGroup : Item
    {
        private List<Item> groupConditions = new List<Item>();

        public ConditionGroup()
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
            if (sb.Length > 0)
            {
                sb.Append(" AND ");
            }

            sb.Append("(");
            foreach (Item item in GroupConditions)
            {
                if(item is ConditionItem)
                {
                    item.LoadParameters(sb, pars, joinType);
                }
                else if (item is ConditionGroup)
                {
                    ConditionGroup group = item as ConditionGroup;
                    foreach (Item sub in group.GroupConditions)
                    {
                        sub.LoadParameters(sb, pars, JoinType.AND);
                    }
                }
            }
            sb.Append(")");
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
