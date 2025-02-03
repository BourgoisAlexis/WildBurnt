using System.Threading.Tasks;

public class ViewWildBurntBasic : UIView {
    protected GameView _gameView;

    public override void Init(params object[] parameters) {
        _gameView = parameters[0] as GameView;
        base.Init(parameters);
    }

    public override async Task Show(bool instant, params object[] parameters) {
        await base.Show(instant, parameters);
        _gameView.ViewLoaded();
    }
}
