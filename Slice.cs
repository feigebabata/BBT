using System;
using System.Collections;
using System.Collections.Generic;

namespace BBT
{
    public sealed class Slice<T>
    {
        public delegate bool Compare(T _obj);
        Dictionary<float,T> m_data;

        public int Count
        {
            get{return m_data.Count;}
        }

        public T this[float _index]
        {
            get
            {
                return fuzzyFind(_index);
            }
            set
            {
                m_data[_index]=value;
            }
        }

        public T[] this[float _index,int _count]
        {
            get
            {
                return fuzzyFinds(_index,_count);
            }
        }

        public List<T> this[float _min,float _max]
        {
            get
            {
                return getRange(_min,_max);
            }
        }

        public Slice(Dictionary<float,T> _data = null)
        {
            if(_data==null)
            {
                m_data = new Dictionary<float, T>();
            }
            else
            {
                m_data = _data;
            }
        }

        public void Clear()
        {
            m_data.Clear();
        }

        public int Remove(float _index,int _count=1)
        {
            var keys = sort(_index);
            int length = Math.Min(keys.Count,m_data.Count);
            for (int i = 0; i < length; i++)
            {
                m_data.Remove(keys[i]);
            }
            return length;
        }

        public int RemoveRange(float _min,int _max)
        {
            var datas = m_data.GetEnumerator();
            int count=0;
            while(datas.MoveNext())
            {
                if(datas.Current.Key >= _min && datas.Current.Key <= _max)
                {
                    count++;
                    m_data.Remove(datas.Current.Key);
                }
            }
            return count;
        }

        public List<float> Find(Compare _compare)
        {
            List<float> idxs = new List<float>();
            var kvs = m_data.GetEnumerator();
            while (kvs.MoveNext())
            {
                if(_compare(kvs.Current.Value))
                {
                    idxs.Add(kvs.Current.Key);
                }
            }
            return idxs;
        }

        List<float> sort(float _index)
        {
            float[] keys = new float[m_data.Keys.Count];
            m_data.Keys.CopyTo(keys,0);
            var keyList = new List<float>(keys);
            keyList.Sort((a,b)=>
            {

                if(Math.Abs(a-_index)>Math.Abs(b-_index))
                {
                    return 1;
                }
                return -1;
            });
            return keyList;
        }

        T fuzzyFind(float _index)
        {
            if(m_data.Count==0)
            {
                return default(T);
            }
            return m_data[sort(_index)[0]];
        }

        T[] fuzzyFinds(float _index,int _count)
        {
            if(m_data.Count==0)
            {
                return null;
            }
            var keys = sort(_index);
            int length = Math.Min(_count,keys.Count);
            T[] arr = new T[length];
            for (int i = 0; i < length; i++)
            {
                arr[i]=m_data[keys[i]];
            }
            return arr;
        }

        List<T> getRange(float _min, float _max)
        {
            List<T> list = new List<T>();
            var datas = m_data.GetEnumerator();
            while(datas.MoveNext())
            {
                if(datas.Current.Key >= _min && datas.Current.Key <= _max)
                {
                    list.Add(datas.Current.Value);
                }
            }
            return list;
        }
    }
}