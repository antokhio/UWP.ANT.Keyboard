### UWP Keyboard Widget with input injection

## Usage:
```
<Page ...
	xmlns:akb="using:UWP.ANT.Keyboard"
	/>
	<akb:KeyboardControl  ButtonStyle="{StaticResource MyKeyboardButtonStyle}" ButtonActiveStyle="{StaticResource MyActiveKeyboardButtonStyle}" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
</Page>
```

## Needs:
```
<Package ... 
	xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities">
	...
	<Capabilities>
		<rescap:Capability Name="inputInjectionBrokered" />
	</Capabilities>
</Package>
```
