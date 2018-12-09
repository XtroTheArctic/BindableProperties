# Bindable Properties

BindableProperties is a class library which provides a bindable properties system similar to "Microsoft WPF Dependency Properties" and "Xamarin Bindable Properties" systems. The biggest difference of BindableProperties to those systems is that it does NOT depend on any other 3rd party platform or framework such as Microsoft WPF and Xamarin so it can be used by almost any kind of .NET projects.

# Features

* Privately settable property class as well as publicly settable one,
* Ability of binding a property to another property (Both privately and publicly),
* Support for empty value,
* Event based value retrieval instead of on-demand value retrieval(classical way) which becomes repetitive and leads to programming errors,
* Mechanism of monitoring other properties (with different value types) for auto-rebindind.

# Team Members

* Project Lead: Onur "Xtro" Er, Atesh Entertainment Inc. onurer@gmail.com

# Download and Install

You can directly install BindableProperties via [NuGet](https://www.nuget.org/packages/Atesh.BindableProperties).

**OR**

Download it via "manual download" link in [NuGet](https://www.nuget.org/packages/Atesh.BindableProperties) web page and extract the assembly into your project manually if you don't want to use a NuGet client.

"nupkg" file you downloaded from NuGet web page is a regular zip file. You can change its extension to "zip" and extract it easily.

# Examples

Example projects about how to use BindableProperties can be downloaded [here](https://drive.google.com/file/d/17gmZnbUdmaalmMSvZUryL-OOmORPIo5A/view?usp=sharing).

# Contribution

You can easily contribute to the project by just reporting issues to [here](https://bitbucket.org/XtroTheArctic/bindableproperties/issues)

If you want to get involved and actively contribute to the project, you can simply do so by sending pull requests to the project lead via bitbucket.com.

Project page on [Bitbucket](https://bitbucket.org/XtroTheArctic/bindableproperties)

Git Repo URL: git@bitbucket.org:XtroTheArctic/bindableproperties.git

Please feel free to contact the team members via email at any time.

# Planned Features

* Optional two-way binding support,
* Value validation support.

# The Unlicense

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or distribute this software, either in source code form or as a compiled binary, for any purpose, commercial or non-commercial, and by any means.

In jurisdictions that recognize copyright laws, the author or authors of this software dedicate any and all copyright interest in the software to the public domain. We make this dedication for the benefit of the public at large and to the detriment of our heirs and successors. We intend this dedication to be an overt act of relinquishment in perpetuity of all present and future rights to this software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>