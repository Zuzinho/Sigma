const editorFormInput = document.getElementById('editorForm')
const editorPreview = document.getElementById('editorPreview')

const highlightActiveTabLink = () => {
  document
      .querySelectorAll('.editor-tabs__item')
      .forEach((element) => element.classList.remove('row__editor_active'))

  document
      .querySelector(`.editor-tabs__item[href="${window.location.hash}"]`)
      .classList.add('row__editor_active')
}
window.addEventListener('load', () => {
  window.location.hash = '#editorForm'
  highlightActiveTabLink()
})

window.addEventListener('hashchange', () => {
  highlightActiveTabLink()
    if (window.location.hash === '#editorPreview') {
        editorPreview.innerHTML = marked.parse(editorFormInput.value)
  }
})