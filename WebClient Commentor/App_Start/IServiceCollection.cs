using System;

namespace WebClient_Commentor.App_Start
{
    public interface IServiceCollection :
        System.Collections.Generic.ICollection<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>,
        System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>,
        System.Collections.Generic.IList<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>
    {
        object AddAuthentication(Action<object> p);
        void AddRazorPages();
        void AddDbContext<T>(Func<object, object> p);

        public string DefaultAuthenticateScheme { get; set; }
    }

}