using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询条件项
    /// </summary>
    public class ConditionItem : Item
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">查询字段(数据库列)名称</param>
        /// <param name="fieldValue">查询字段值</param>
        /// <param name="conditionType">字段查询类别</param>
        public ConditionItem(string fieldName, object fieldValue, SearchType conditionType)
        {
            this.ItemType = ItemType.ConditionItem;

            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ConditionType = conditionType;
        }

        public override void LoadParameters(StringBuilder sb,List<DbParameter> pars,JoinType joinType)
        {
            String fieldName = FieldName;
            String parameterName = FieldName.Replace("(", "").Replace(")", "").
                Replace("&", "").Replace("|", "").Replace("+", "").
                Replace("-", "").Replace("*", "").Replace("/", "");

            //检测参数是否有重复项
            if (pars.Exists(p => p.ParameterName == parameterName))
            {
                parameterName += Guid.NewGuid().ToString("N");
            }

            if (sb.Length >3 && !sb.ToString().EndsWith("("))//避免一上来就是AND/OR连接符；避免左括号直接跟AND/OR连接符
            {
                sb.Append(" " + joinType.ToString() + " ");   
            }

            switch (ConditionType)
            {
                case SearchType.Accurate:
                    if (FieldValue == null)
                    {
                        sb.Append(fieldName + " IS NULL");
                    }
                    else
                    {
                        sb.Append(fieldName + "=@" + parameterName);
                        pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    }
                    break;
                case SearchType.Fuzzy:
                    sb.Append(fieldName + " LIKE @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, "%" + FieldValue + "%"));
                    break;
                case SearchType.LeftFuzzy:
                    sb.Append(fieldName + " LIKE '%'" + CommonVar.JoinTag + "@" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                case SearchType.RightFuzzy:
                    sb.Append(fieldName + " LIKE @" + parameterName + CommonVar.JoinTag + "'%'");
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                case SearchType.UnContains:
                    sb.Append(fieldName + " NOT LIKE @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, "%" + FieldValue + "%"));
                    break;
                case SearchType.Unequal:
                    if (FieldValue == null)
                    {
                        sb.Append(fieldName + " IS NOT NULL");
                    }
                    else
                    {
                        sb.Append(fieldName + " <> @" + parameterName);
                        pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    }
                    break;
                case SearchType.ValuesIn:
                    sb.Append("(");
                    String[] fiels = FieldValue.ToString().Split(',');
                    for (int i = 0; i < fiels.Length; i++)
                    {
                        if (i == (fiels.Length - 1))
                        {
                            sb.Append(fieldName + " = @" + (parameterName + i));
                        }
                        else
                        {
                            sb.Append(fieldName + " = @" + (parameterName + i) + " OR ");
                        }
                        pars.Add(SQLFactory.CreateParameter((parameterName + i), fiels[i]));
                    }
                    sb.Append(")");
                    break;
                case SearchType.ValuesNotIn:
                    sb.Append("(");
                    fiels = FieldValue.ToString().Split(',');
                    for (int i = 0; i < fiels.Length; i++)
                    {
                        if (i == (fiels.Length - 1))
                        {
                            sb.Append(FieldName + " <> @" + (FieldName + i));
                        }
                        else
                        {
                            sb.Append(FieldName + " <> @" + (FieldName + i) + " AND ");
                        }
                        pars.Add(SQLFactory.CreateParameter((parameterName + i), fiels[i]));
                    }
                    sb.Append(")");
                    break;
                case SearchType.Greater:
                    sb.Append(fieldName + " > @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                case SearchType.Lower:
                    sb.Append(fieldName + " < @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                case SearchType.GreaterEqual:
                    sb.Append(fieldName + " >= @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                case SearchType.LowerEqual:
                    sb.Append(fieldName + " <= @" + parameterName);
                    pars.Add(SQLFactory.CreateParameter(parameterName, FieldValue));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询项字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 查询项值
        /// </summary>
        public object FieldValue { get; set; }

        /// <summary>
        /// 查询条件类型
        /// </summary>
        public SearchType ConditionType { get; set; }
    }

    public enum SearchType
    {
        /// <summary>
        /// 精确查询
        /// </summary>
        Accurate,

        /// <summary>
        /// 模糊查询
        /// </summary>
        Fuzzy,

        /// <summary>
        /// 左侧模糊查询
        /// </summary>
        LeftFuzzy,

        /// <summary>
        /// 右侧模糊查询
        /// </summary>
        RightFuzzy,

        /// <summary>
        /// 不包含
        /// </summary>
        UnContains,

        /// <summary>
        /// 不等于
        /// </summary>
        Unequal,

        /// <summary>
        /// 包含其中值
        /// </summary>
        ValuesIn,

        /// <summary>
        /// 不包含其中值
        /// </summary>
        ValuesNotIn,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 小于
        /// </summary>
        Lower,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// 小于等于
        /// </summary>
        LowerEqual
    }
}
