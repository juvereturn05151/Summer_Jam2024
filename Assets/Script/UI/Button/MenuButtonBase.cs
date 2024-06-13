/* filename MenuButtonBase.cs
 * author   Tonnattan Chankasemporn
 * email:   juvereturn@gmail.com
 * 
 * Brief Description: 
 * This class contains fundamentals methods and properties for button that appears in the Main Scene Page (Possibly for buttons in the setting and pause menu as well in the future)
 * This class is expected to be attached to the scene object that has "Button and Image Components" Where Image Component is expected to have ShinyButton Material
 * */

using UnityEngine; /*MonoBehaviour*/
using UnityEngine.UI; /*Button*/
using UnityEngine.EventSystems;/*ISelectHandler*/

public class MenuButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button _buttonComponent;

    private Material _shinyButtonMaterial;
    private const string CYCLE_TIME_ATTRIBUTE_NAME = "_CycleTime";
    private const float CYCLE_TIME_ON_HILIGHT = 2.0f;
    private const float CYCLE_TIME_ON_DEHILIGHT = 0.0f;

    private void Start()
    {
        FindButtonComponent();
        FindShinyMaterial();
    }

    /* Brief
     * Occur when a player hovers a mouse cursor on top of the button
     */

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivateHighlight();
    }

    /* Brief
     * Occur when a player move a mouse cursor away from the button
     */

    public void OnPointerExit(PointerEventData eventData)
    {
        DeactivateHighlight();
    }

    private void ActivateHighlight()
    {
        _shinyButtonMaterial.SetFloat(CYCLE_TIME_ATTRIBUTE_NAME, CYCLE_TIME_ON_HILIGHT);
    }

    private void DeactivateHighlight()
    {
        _shinyButtonMaterial.SetFloat(CYCLE_TIME_ATTRIBUTE_NAME, CYCLE_TIME_ON_DEHILIGHT);
    }

    /* Brief
     * Find the button component, in case we forget to assign in the editor
     */

    private void FindButtonComponent() 
    {
        if (_buttonComponent == null) 
        {
            _buttonComponent = GetComponent<Button>();
        }
    }

    /* Brief
     * Create a duplicate material in order to make the shader independent from each other.
     */

    private void FindShinyMaterial()
    {
        if (GetComponent<Image>() is Image image && image.material != null) 
        {
            image.material = new Material(image.material);
            _shinyButtonMaterial = image.material;
        }
    }
}
