using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	public Color color;
	private Slider hueSlider;
	private Slider saturationSlider;
	private Slider valueSlider;
	private Slider alphaSlider;
	private GameObject colorPreview;
	private float H, S, V;
	private Vector2 textureCoord;
	private bool hitPixel;
	
	void Awake()
	{
		hueSlider = transform.Find("Hue (Slider)").GetComponent<Slider>();
		saturationSlider = transform.Find("Saturation (Slider)").GetComponent<Slider>();
		valueSlider = transform.Find("Value (Slider)").GetComponent<Slider>();
		alphaSlider = transform.Find("Alpha (Slider)").GetComponent<Slider>();
		colorPreview = transform.Find("Color Preview").gameObject;
		
		ChangeColor();
	}
	
	void Update()
	{
		//Cast ray and get pixel via color picker
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Color")))
		{
			if (hit.transform.gameObject == transform.Find("Color").gameObject)
			{
				if (Input.GetMouseButtonDown(0))
				{
					hitPixel = true;
				}
				
				if (Input.GetMouseButton(0))
				{
					textureCoord = hit.textureCoord;
				}
			}
		}
		else
		{
			hitPixel = false;
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			hitPixel = false;
		}
		
		if (hitPixel == true)
		{
			//Pick color
			color = Color.HSVToRGB(H, textureCoord.x, textureCoord.y);
			
			SetColor();
		}
	}
	
	public void ChangeColor()
	{
		//Color as HSV
		color = Color.HSVToRGB(hueSlider.value, saturationSlider.value, valueSlider.value);
		Color.RGBToHSV(color, out H, out S, out V);
		
		GetColor();
		
		//Color preview
		colorPreview.GetComponent<RectTransform>().anchoredPosition = new Vector2(S * transform.Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.x, (V * GameObject.Find("Canvas").transform.Find("Color Picker").Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.y) - GameObject.Find("Canvas").transform.Find("Color Picker").Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.y);
		colorPreview.GetComponent<Image>().color = color;
	}
	
	void SetColor()
	{
		//Color as HSV
		Color.RGBToHSV(color, out H, out S, out V);
		
		//Set slider values
		hueSlider.value = H;
		saturationSlider.value = S;
		valueSlider.value = V;
		
		//TextureCoord
		textureCoord = new Vector2(S, V);
		
		//Color preview
		colorPreview.GetComponent<RectTransform>().anchoredPosition = new Vector2(textureCoord.x * transform.Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.x, (textureCoord.y * GameObject.Find("Canvas").transform.Find("Color Picker").Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.y) - GameObject.Find("Canvas").transform.Find("Color Picker").Find("Color (Image)").GetComponent<RectTransform>().sizeDelta.y);
		colorPreview.GetComponent<Image>().color = color;
		
		GetColor();
	}
	
	void GetColor()
	{
		//Color as RGB + alpha
		color = new Color(color.r, color.g, color.b, alphaSlider.value);
		
		//Change handle colors
		hueSlider.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
		saturationSlider.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
		valueSlider.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = Color.HSVToRGB(H, 1, V);
		alphaSlider.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
		
		//Change background
		saturationSlider.gameObject.transform.Find("White Background").GetComponent<Image>().color = Color.HSVToRGB(0, 0, V);
		saturationSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(H, 1, V);
		valueSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.HSVToRGB(H, 1, V);
		alphaSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);
		
		//Change color color
		transform.Find("Color (Image)").GetComponent<Image>().color = Color.HSVToRGB(H, 1, 1);
	}
}
