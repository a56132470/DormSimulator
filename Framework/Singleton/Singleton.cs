namespace DSD.Framework.Singleton
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_Instance;

        private static readonly object syslock = new object();

        public static T Instance
        {
            get
            {
                lock (syslock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new T();
                    }
                }
                return m_Instance;
            }
        }
        public virtual void Init()
        {

        }
    }
}

