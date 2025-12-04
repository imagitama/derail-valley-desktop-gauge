# 1.5.0

- will not start if already running
- added settings buttons to edit configs
- added settings buttons to control OpenSimGauge
- added very basic Engine RPM gauge
- added Reverser gauge

Note: Will backup your config and gauges automatically on mod launch.

# 1.4.0

- added strict dependency on mod `DerailValleyWebSocket`

# 1.3.1

- fixed crash because of `transition` prop

# 1.3.0

- converted into a mod named `DerailValleyDesktopGauge`:

Now it bundles OpenSimGauge + panels/gauges into a single Derail Valley mod so it all "just works".

# 1.2.0

- added var `train_brake` (`position`)

# 1.1.0

- added var `throttle` (`position`)

# 1.0.3

- add car type into name
- added port setting

# 1.0.2

- fixed emit pausing on loading a new game
- fixed crash on return to main menu
- fixed `System.InvalidOperationException: Collection was modified; enumeration operation may not execute.` error
- improved WebSocket error handling and logging

# 1.0.1

- changed car speedo to use UI if available, otherwise use port, otherwise use absolute speed

# 1.0.0

Initial version
