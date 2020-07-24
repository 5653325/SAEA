﻿/****************************************************************************
*项目名称：SAEA.RedisSocket.Core
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.RedisSocket.Core
*类 名 称：RedisHashOperation
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2019/8/14 15:53:12
*描述：
*=====================================================================
*修改时间：2019/8/14 15:53:12
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.RedisSocket.Model;
using System.Collections.Generic;

namespace SAEA.RedisSocket.Core
{
    /// <summary>
    /// Hash操作
    /// </summary>
    public partial class RedisDataBase
    {
        /// <summary>
        /// 将哈希表 hash 中域 field 的值设置为 value 
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void HSet(string hid, string key, string value)
        {
            _cnn.DoWithID(RequestType.HSET, hid, key, value);
        }
        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="keyvalues"></param>
        public void HMSet(string hid, Dictionary<string, string> keyvalues)
        {
            _cnn.DoBatchWithIDDic(RequestType.HMSET, hid, keyvalues);
        }
        /// <summary>
        /// 返回哈希表中给定域的值
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string HGet(string hid, string key)
        {
            return _cnn.DoWithKeyValue(RequestType.HGET, hid, key).Data;
        }

        /// <summary>
        /// 返回哈希表 key 中，一个或多个给定域的值
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<string> HMGet(string hid, List<string> keys)
        {
            return _cnn.DoBatchWithList(RequestType.HMGET, hid, keys).ToList();
        }
        /// <summary>
        /// 返回哈希表 key 中，所有的域和值
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public Dictionary<string, string> HGetAll(string hid)
        {
            return _cnn.DoWithKey(RequestType.HGETALL, hid).ToKeyValues();
        }

        /// <summary>
        /// 返回哈希表 key 中的所有域
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<string> HGetKeys(string hid)
        {
            return _cnn.DoWithKey(RequestType.HKEYS, hid).ToList();
        }

        /// <summary>
        /// 返回哈希表 key 中所有域的值
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public List<string> HGetValues(string hid)
        {
            return _cnn.DoWithKey(RequestType.HVALS, hid).ToList();
        }

        /// <summary>
        /// 删除哈希表 key 中的一个或多个指定域，不存在的域将被忽略
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseData HDel(string hid, string key)
        {
            return _cnn.DoWithKeyValue(RequestType.HDEL, hid, key);
        }

        /// <summary>
        /// 删除哈希表 key 中的一个或多个指定域，不存在的域将被忽略
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public ResponseData HDel(string hid, string[] keys)
        {
            return _cnn.DoBatchWithIDKeys(RequestType.HDEL, hid, keys);
        }

        /// <summary>
        /// 返回哈希表 key 中域的数量
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public int HLen(string hid)
        {
            var result = 0;
            int.TryParse(_cnn.DoWithKey(RequestType.HLEN, hid).Data, out result);
            return result;
        }
        /// <summary>
        /// 检查给定域 field 是否存在于哈希表 hash 当中
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HExists(string hid, string key)
        {
            var result = _cnn.DoWithKeyValue(RequestType.HEXISTS, hid, key).Data;
            return result == "1" ? true : false;
        }

        /// <summary>
        /// 返回哈希表 key 中， 与给定域 field 相关联的值的字符串长度
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public int HStrLen(string hid, string key)
        {
            var result = 0;

            int.TryParse(_cnn.DoWithKeyValue(RequestType.HSTRLEN, hid, key).Data, out result);

            return result;
        }

        /// <summary>
        /// 为哈希表 key 中的域 field 的值加上增量 increment 
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public long HIncrementBy(string hid, string key, int num)
        {
            return long.Parse(_cnn.DoWithID(RequestType.HINCRBY, hid, key, num.ToString()).Data);
        }

        /// <summary>
        /// 为哈希表 key 中的域 field 加上浮点数增量 increment
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public float HIncrementByFloat(string hid, string key, float num)
        {
            return float.Parse(_cnn.DoWithID(RequestType.HINCRBYFLOAT, hid, key, num.ToString()).Data);
        }

    }
}
