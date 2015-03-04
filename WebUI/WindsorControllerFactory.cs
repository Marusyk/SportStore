using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Core;

namespace WebUI
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        WindsorContainer container;
        // Конструктор:
        // 1. Встановлює новий контейнер ІоС
        // 2. Реєструє всі компоненти, спеціалізовані в web.config
        // 3. Реєструє всі типи контролерів в якості компонентів
        public WindsorControllerFactory()
        {
            //Створює екземпляр контейнера, взяв конфігураціб з web.config
            container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

            //Зареєструвати всі типи контродерів як Transient
            var controllerTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                  where typeof(IController).IsAssignableFrom(t)
                                  select t;
            foreach (Type t in controllerTypes)
                container.Register(Component.For(t).Named(t.FullName).LifeStyle.Is(LifestyleType.Transient));
        }

        // Конструюємо екземпляр контейнера, який необхідний для обслуговування кожного запиту
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return (IController)container.Resolve(controllerType);
        }
    }
}