﻿using Newtonsoft.Json;
using Puppetry.Puppeteer.Conditions;
using Puppetry.Puppeteer.Utils;

namespace Puppetry.Puppeteer
{
    public class GameObject
    {
        private string _uPath;
        internal string LocatorMessage { get; private set;}

        public GameObject() { }

        public GameObject(string root, string name)
        {
            _uPath = $"/{root}//{name}";
            LocatorMessage = $"root: {root} and name: {name}";
        }

        public GameObject(string root, string name, string parent)
        {
            _uPath = $"/{root}//{parent}//{name}";
            LocatorMessage = $"root: {root}, name: {name} and parent: {parent}";
        }

        public GameObject FindByUPath(string upath)
        {
            _uPath = upath;
            LocatorMessage = $"upath: {_uPath}";

            return this;
        }
        
        public GameObject FindByName(string root, string name)
        {
            _uPath = $"/{root}//{name}";
            LocatorMessage = $"root: {root} and name: {name}";

            return this;
        }
        
        public GameObject FindByNameAndParent(string root, string name, string parent)
        {
            _uPath = $"/{root}//{parent}//{name}";
            LocatorMessage = $"root: {root}, name: {name} and parent: {parent}";

            return this;
        }

        public GameObject FindRelative(string upath)
        {
            return new GameObject().FindByUPath(_uPath + upath);
        }
        
        public bool Exists => PuppetryDriver.Instance.Exist(_uPath);
        
        public bool IsActiveInHierarchy => PuppetryDriver.Instance.Active(_uPath, LocatorMessage);
        
        public bool IsRendering => PuppetryDriver.Instance.IsRendering(_uPath, LocatorMessage);
        
        public bool IsOnScreen => PuppetryDriver.Instance.IsOnScreen(_uPath, LocatorMessage);
        
        public bool IsHitByGraphicRaycast => PuppetryDriver.Instance.IsHitByGraphicRaycast(_uPath, LocatorMessage);
        
        public bool IsHitByPhysicsRaycast => PuppetryDriver.Instance.IsHitByPhysicsRaycast(_uPath, LocatorMessage);
        
        public int Count => PuppetryDriver.Instance.Count(_uPath);

        public void Click()
        {
            PuppetryDriver.Instance.Click(_uPath, LocatorMessage);
        }

        public void SendKeys(string value)
        {
            PuppetryDriver.Instance.SendKeys(value, _uPath, LocatorMessage);
        }

        public void Swipe(Constants.Directions direction)
        {
            PuppetryDriver.Instance.Swipe(_uPath, direction.ToString().ToLower(), LocatorMessage);
        }
        
        public void DragTo(ScreenCoordinates toCoordinates)
        {
            PuppetryDriver.Instance.DragTo(_uPath, JsonConvert.SerializeObject(toCoordinates), LocatorMessage);
        }
        
        public void DragTo(GameObject toGameObject)
        {
            var toCoordinates = toGameObject.GetScreenCoordinates();
            DragTo(toCoordinates);
        }

        public string GetComponent(string component)
        {
            return PuppetryDriver.Instance.GetComponent(_uPath, component, LocatorMessage);
        }
        
        public ScreenCoordinates GetScreenCoordinates()
        {
            var result = PuppetryDriver.Instance.GetCoordinates(_uPath, LocatorMessage);

            return JsonConvert.DeserializeObject<ScreenCoordinates>(result);
        }

        public void Should(Condition condition, int timeoutMs)
        {
            Wait.For(() => condition.Invoke(this),
                () =>
                    $"Timed out after {timeoutMs / 1000} seconds while waiting for Condition: {condition}",
                Configuration.TimeoutMs);
        }

        public void Should(Condition condition)
        {
            Should(condition, Configuration.TimeoutMs);
        }

        public void ShouldNot(Condition condition, int timeoutMs)
        {
            Wait.For(() => !condition.Invoke(this),
                () =>
                    $"Timed out after {timeoutMs / 1000} seconds while waiting for Condition Not fulfilled: {condition}",
                Configuration.TimeoutMs);
        }

        public void ShouldNot(Condition condition)
        {
            ShouldNot(condition, Configuration.TimeoutMs);
        }
    }
}
